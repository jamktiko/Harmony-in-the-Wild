using System;
using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.Extensions;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.Services;
using UnityEngine;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// Draws connecting lines between each pair of points.
    /// </summary>
    [ScriptNode("Draw Connections (Biased Random Walk)", "Drawing"), Serializable]
    public class DrawConnectionsBiasedRandomWalkNode : ProcessorNode
    {
        [InPort("Texture", typeof(TextureData), true), SerializeReference]
        private InPort textureIn = null;
        
        [InPort("Connected Points", typeof(Pair<Vector2Int>[]), true), SerializeReference] 
        private InPort connectionsIn = null;
        
        [InPort("Draw Color", typeof(Color32)), SerializeReference]
        private InPort drawColorIn = null;

        [InPort("Carve Radius", typeof(int)), SerializeReference]
        private InPort radiusIn = null;

        [InPort("Direction Bias (%)", typeof(float)), SerializeReference]
        private InPort biasIn = null;

        [InPort("Randomness (%)", typeof(float)), SerializeReference]
        private InPort randomnessIn = null;


        [OutPort("Texture", typeof(TextureData)), SerializeReference] 
        private OutPort textureOut = null;
        
        
        private TextureData textureData;

#if UNITY_EDITOR
        /// <summary>
        /// Gets the latest generated texture data. Only available in the editor.
        /// </summary>
        public TextureData TextureData => textureData;
#endif

        private Vector2Int currentPoint;
        private Vector2Int destination;
        private int width;
        private int height;
        private float bias;
        private float randomness;
        
        private List<Vector2Int> nextPoints;
        private SortedList<float, Vector2Int> sortedNextPoints;
        private List<Vector2Int> bestOptions;

        private Rng rng;
        
        private float NextPointWeight(Vector2Int point)
        {
            var directionPoint = point - currentPoint;
            var directionDestination = destination - currentPoint;

            var angle = Vector2.Angle(directionPoint, directionDestination);
            return .25f + .75f * bias - angle * bias / 180;
        }

        private bool IsPointValid(Vector2Int point) 
            => point.x >= 0 && point.x < width && point.y >= 0 && point.y < height;

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey itemA, TKey itemB)
            {
                var result = itemA.CompareTo(itemB);

                return result == 0 ? 1 : result;
            }
        }

        private Vector2Int Next()
        {
            nextPoints.Clear();
            sortedNextPoints.Clear();
            bestOptions.Clear();
            
            var north = new Vector2Int(currentPoint.x, currentPoint.y + 1);
            var south = new Vector2Int(currentPoint.x, currentPoint.y - 1);
            var west = new Vector2Int(currentPoint.x - 1, currentPoint.y);
            var east = new Vector2Int(currentPoint.x + 1, currentPoint.y);
            
            if (IsPointValid(north)) nextPoints.Add(north);
            if (IsPointValid(south)) nextPoints.Add(south);
            if (IsPointValid(west)) nextPoints.Add(west);
            if (IsPointValid(east)) nextPoints.Add(east);

            var totalWeight = 0f;

            foreach (var point in nextPoints)
            {
                var pointWeight = NextPointWeight(point);
                sortedNextPoints.Add(pointWeight, point);
                totalWeight += pointWeight;
            }

            var nextPoint = sortedNextPoints.Values[sortedNextPoints.Count - 1];
            var pickBest = rng.NextFloat() > randomness;
            if (pickBest)
            {
                // Simply select one of the best options
                if (sortedNextPoints.Count == 1) return sortedNextPoints.Values[0];

                var highestWeight = sortedNextPoints.Keys[sortedNextPoints.Count - 1];
                bestOptions.Add(sortedNextPoints.Values[sortedNextPoints.Count - 1]);

                for (var i = sortedNextPoints.Count - 2; i >= 0; --i)
                {
                    var pointWeight = sortedNextPoints.Keys[i];
                    if (pointWeight < highestWeight) break;
                    
                    var point = sortedNextPoints.Values[i];
                    bestOptions.Add(point);
                }
                
                var randomIndex = rng.Next(0, bestOptions.Count);
                return bestOptions[randomIndex];
            }

            var weight = rng.NextFloat(0, totalWeight);
            var currentWeight = 0f;
            foreach (var sortedNextPoint in sortedNextPoints)
            {
                var pointWeight = sortedNextPoint.Key;
                var point = sortedNextPoint.Value;
            
                currentWeight += pointWeight;

                if (weight > currentWeight) continue;
                
                nextPoint = point;
                break;
            }

            return nextPoint;
        }
        
        /// <inheritdoc cref="ProcessorNode.OnProcess"/>
        protected override void OnProcess()
        {
            rng = Get<Rng>();
            
            var instanceProvider = Get<IInstanceProvider>();

            textureData = instanceProvider.Get<TextureData>();
            textureIn.Get<TextureData>().Clone(textureData);
            
            bias = biasIn.IsConnected ? biasIn.Get<float>() / 100 : .5f;
            randomness = randomnessIn.IsConnected ? randomnessIn.Get<float>() / 100 : 1f;
            var connections = connectionsIn.Get<Pair<Vector2Int>[]>();
            var color = drawColorIn.Get<Color32>();
            var radius = radiusIn.IsConnected ? radiusIn.Get<int>() : 1;

            width = textureData.Width;
            height = textureData.Height;

            nextPoints = instanceProvider.Get<List<Vector2Int>>();
            nextPoints.EnsureCapacity(4);

            if (sortedNextPoints == null)
            {
                sortedNextPoints = new SortedList<float, Vector2Int>(new DuplicateKeyComparer<float>());
            }
            sortedNextPoints.EnsureCapacity(nextPoints.Capacity);
            
            bestOptions = instanceProvider.Get<List<Vector2Int>>();
            bestOptions.EnsureCapacity(nextPoints.Capacity);

            var carvePoints = instanceProvider.Get<List<Vector2Int>>();
            
            foreach (var connection in connections)
            {
                currentPoint = connection.First;
                destination = connection.Second;
                
                var startIndex = width * currentPoint.y + currentPoint.x;
                textureData[startIndex] = color;

                while (currentPoint != destination)
                {
                    currentPoint = Next();
                    carvePoints.Add(currentPoint);
                }
            }

            if (radius > 1)
            {
                var relativePoints = instanceProvider.Get<List<Vector2Int>>();
                textureData.DrawCircles(carvePoints, radius, color, relativePoints);
            }
            else
            {
                foreach (var point in carvePoints)
                {
                    var currentIndex = width * point.y + point.x;
                    textureData[currentIndex] = color;
                }
            }

            textureOut.Set(() => textureData);
        } 
    }
}