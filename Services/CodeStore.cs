// Services/CodeStoreService.cs
using System.Collections.Generic;

namespace youtubeApi.Services {

    public class CodeStore
{
    private readonly Dictionary<string, string> _codeStore = new();

    public void StoreCode(string email, string code)
    {
        _codeStore[email] = code;
    }

    public string? GetCode(string email)
    {
        _codeStore.TryGetValue(email, out var code);
        return code;
    }
}

}