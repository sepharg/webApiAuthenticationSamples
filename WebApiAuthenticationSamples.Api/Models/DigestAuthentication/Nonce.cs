using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace WebApiAuthenticationSamples.Api.Models.DigestAuthentication
{
    public class Nonce
    {
        private static readonly ConcurrentDictionary<string, Tuple<int, DateTime>> nonces = new ConcurrentDictionary<string, Tuple<int, DateTime>>();

        public static string Generate()
        {
            byte[] bytes = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            string nonce = bytes.ToMD5Hash();

            nonces.TryAdd(nonce, new Tuple<int, DateTime>(0, DateTime.Now.AddMinutes(10)));

            return nonce;
        }

        public static bool IsValid(string nonce, string nonceCount)
        {
            Tuple<int, DateTime> cachedNonce = null;
            nonces.TryGetValue(nonce, out cachedNonce);

            // nonce is found and nonce count is greater than the one in record
            if (Int32.Parse(nonceCount) > cachedNonce?.Item1)
            {
                // nonce has not expired yet
                if (cachedNonce.Item2 > DateTime.Now)
                {
                    // update the dictionary to reflect the nonce
                    // count just received in this request
                    nonces[nonce] = new Tuple<int, DateTime>(cachedNonce.Item1 + 1, cachedNonce.Item2);

                    // everything seems to be fine
                    // server nonce is fresh and nonce count seems
                    // to be incremented. Does not look like replay.
                    return true;
                }
            }
            return false;
        }
    }
}