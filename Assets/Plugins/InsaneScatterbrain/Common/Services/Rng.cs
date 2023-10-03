using System;
using System.Threading;
using InsaneScatterbrain.RandomNumberGeneration;

namespace InsaneScatterbrain.Services
{
    /// <summary>
    /// Conveniently wrapped ThreadLocal&lt;Random&gt; class.
    /// </summary>
    public class Rng : Random
    {
        private readonly ThreadLocal<RandomNumberGeneration.Rng> random;

        public RandomNumberGeneration.Rng InnerRng => random.Value;

        public Rng(int seed)
        {
            random = new ThreadLocal<RandomNumberGeneration.Rng>(() => new RandomNumberGeneration.Rng(RngState.FromInt(seed)));
        }

        public Rng(Guid seed)
        {
            random = new ThreadLocal<RandomNumberGeneration.Rng>(() =>
                new RandomNumberGeneration.Rng(RngState.FromBytes(seed.ToByteArray())));
        }

        public Rng()
        {
            random = new ThreadLocal<RandomNumberGeneration.Rng>(() => new RandomNumberGeneration.Rng(RngState.New()));
        }

        public override int Next() => random.Value.Int();
    
        public override int Next(int minValue, int maxValue) => random.Value.Int(minValue, maxValue);
    
        public override int Next(int maxValue) => random.Value.Int(maxValue);

        public override void NextBytes(byte[] buffer) => random.Value.Bytes(buffer);
    
        public override double NextDouble() => random.Value.Double();
    }
}