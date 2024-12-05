/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using Unity.Mathematics;

namespace RPSCore {

    public class SeedHandler : UnityEngine.MonoBehaviour {

        private Random random;
        private uint seed;


        #region Seed Config
        public void SetSeed(uint newSeed) {
            seed = newSeed;
            random = new Random(seed);
        }

        public uint GetSeed() { return seed; }

        public float GetSeedAsFloat() {
            byte[] uintBytes = System.BitConverter.GetBytes(seed);
            return System.BitConverter.ToSingle(uintBytes, 0);
        }
        #endregion

        #region Seeded value fetches (deterministic RNG)
        public int Range(int min, int max) {
            return random.NextInt(min, max);
        }

        public float Range(float min, float max) {
            return random.NextFloat(min, max);
        }

        public int RandomInt() {
            return random.NextInt();
        }

        public uint RandomUInt() {
            return random.NextUInt();
        }

        public float RandomFloat() {
            return random.NextFloat();
        }

        public float2 RandomFloat2() {
            return random.NextFloat2();
        }

        public float3 RandomFloat3() {
            return random.NextFloat3();
        }

        public bool RandomBool() {
            return random.NextBool();
        }
        #endregion

        #region Non-seeded value fetches (pure RNG)
        public int NonSeededRandomInt(int min, int max) {
            Random random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(0, int.MaxValue));
            return random.NextInt(min, max);
        }

        public uint NonSeededRandomUInt() {
            Random random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(0, int.MaxValue));
            return random.NextUInt();
        }

        public uint NonSeededRandomUInt(uint min, uint max) {
            Random random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(0, int.MaxValue));
            return random.NextUInt(min, max);
        }
        #endregion

    }

}