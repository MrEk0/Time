using System;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Common
{
    public static class Deserializer
    {
        [CanBeNull]
        public static T DeserializeFromJson<T>(string json, T def = default)
        {
            return TryDeserializeFromJson<T>(json, out var result) ? result : def;
        }

        private static bool TryDeserializeFromJson<T>(string json, out T result)
        {
            try
            {
                result = JsonUtility.FromJson<T>(json);
                return result != null;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);

                result = default;
                return false;
            }
        }
    }
}
