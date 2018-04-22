using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MightyPirates
{
    [CreateAssetMenu]
    public sealed class Sounds : ScriptableObject
    {
        public enum SoundType
        {
            None,
            MeleeAttack,
            GunAttack,
            LaserAttack,
            TakeDamage,
            SmallDeath,
            LargeDeath,
        }

        [SerializeField]
        public AudioClip[] m_MeleeAttackSounds;

        [SerializeField]
        private AudioClip[] m_GunAttackSounds;

        [SerializeField]
        private AudioClip[] m_LaserAttackSounds;

        [SerializeField]
        private AudioClip[] m_TakeDamageSounds;

        [SerializeField]
        private AudioClip[] m_SmallDeathSounds;

        [SerializeField]
        private AudioClip[] m_LargeDeathSounds;

        private static readonly Dictionary<AudioClip[], float> LastPlayedTimes = new Dictionary<AudioClip[], float>();

        public static void Play(SoundType soundType, float volume = 1f)
        {
            AudioClip audioClip = Get(soundType);
            if (audioClip != null)
            {
                PlayerCamera.GlobalAudioSource.PlayOneShot(audioClip, volume);
            }
        }

        public static AudioClip MeleeAttackSound => Get(s_Instance.m_MeleeAttackSounds);
        public static AudioClip GunAttackSound => Get(s_Instance.m_GunAttackSounds);
        public static AudioClip LaserAttackSound => Get(s_Instance.m_LaserAttackSounds);
        public static AudioClip TakeDamageSound => Get(s_Instance.m_TakeDamageSounds);
        public static AudioClip SmallDeathSound => Get(s_Instance.m_SmallDeathSounds);
        public static AudioClip LargeDeathSound => Get(s_Instance.m_LargeDeathSounds);

        public static AudioClip Get(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.None:
                    return null;
                case SoundType.MeleeAttack:
                    return MeleeAttackSound;
                case SoundType.GunAttack:
                    return GunAttackSound;
                case SoundType.LaserAttack:
                    return LaserAttackSound;
                case SoundType.TakeDamage:
                    return TakeDamageSound;
                case SoundType.SmallDeath:
                    return SmallDeathSound;
                case SoundType.LargeDeath:
                    return LargeDeathSound;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
        }

        private static AudioClip Get(AudioClip[] clips)
        {
            float lastPlayedTime;
            if (LastPlayedTimes.TryGetValue(clips, out lastPlayedTime) && Time.time - lastPlayedTime < 0.05f)
                return null;

            LastPlayedTimes[clips] = Time.time;
            return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
        }

        private static Sounds s_Instance;

        private void OnEnable()
        {
            s_Instance = this;
        }

        private void OnDisable()
        {
            s_Instance = null;
        }
    }
}