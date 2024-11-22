using System.Collections.Concurrent;

namespace CarStore.Services
{
    public class TokenBlacklistService
    {
        private readonly ConcurrentBag<string> _blacklistedTokens = new();

        public void AddToken(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }
    }
}
