using System;

namespace MightyPirates
{
    public sealed class FractalNoise
    {
        private readonly OpenSimplexNoise m_Noise;
        private float m_Persistence = 0.5f;
        private int m_Octaves = 3;
        private float m_Period = 64;
        private float m_Lacunarity = 3f;

        public FractalNoise(OpenSimplexNoise noise)
        {
            if (noise == null)
                throw new ArgumentNullException(nameof(noise));

            m_Noise = noise;
        }

        public OpenSimplexNoise Noise => m_Noise;

        public float Persistence
        {
            get { return m_Persistence; }
            set { m_Persistence = value; }
        }

        public int Octaves
        {
            get { return m_Octaves; }
            set { m_Octaves = value; }
        }

        public float Period
        {
            get { return m_Period; }
            set { m_Period = value; }
        }

        public float Lacunarity
        {
            get { return m_Lacunarity; }
            set { m_Lacunarity = value; }
        }

        public double Evaluate(double x, double y)
        {
            x /= m_Period;
            y /= m_Period;

            double amplitude = 1.0;
            double range = 1.0;
            double sum = m_Noise.Evaluate(x, y);

            uint i = 0;
            while (++i < m_Octaves)
            {
                x *= m_Lacunarity;
                y *= m_Lacunarity;
                amplitude *= m_Persistence;
                range += amplitude;
                x += m_Period;
                sum += m_Noise.Evaluate(x, y) * amplitude;
            }

            return sum / range;
        }

        public double Evaluate(double x, double y, double z)
        {
            x /= m_Period;
            y /= m_Period;
            z /= m_Period;

            double amplitude = 1.0;
            double range = 1.0;
            double sum = m_Noise.Evaluate(x, y, z);

            uint i = 0;
            while (++i < m_Octaves)
            {
                x *= m_Lacunarity;
                y *= m_Lacunarity;
                z *= m_Lacunarity;
                amplitude *= m_Persistence;
                range += amplitude;
                x += m_Period;
                sum += m_Noise.Evaluate(x, y, z) * amplitude;
            }

            return sum / range;
        }

        public double Evaluate(double x, double y, double z, double w)
        {
            x /= m_Period;
            y /= m_Period;
            z /= m_Period;
            w /= m_Period;

            double amplitude = 1.0;
            double range = 1.0;
            double sum = m_Noise.Evaluate(x, y, z, w);

            uint i = 0;
            while (++i < m_Octaves)
            {
                x *= m_Lacunarity;
                y *= m_Lacunarity;
                z *= m_Lacunarity;
                w *= m_Lacunarity;
                amplitude *= m_Persistence;
                range += amplitude;
                x += m_Period;
                sum += m_Noise.Evaluate(x, y, z, w) * amplitude;
            }

            return sum / range;
        }
    }
}