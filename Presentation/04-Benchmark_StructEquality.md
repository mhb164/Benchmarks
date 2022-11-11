Your own 𝑰𝑬𝒒𝒖𝒂𝒕𝒂𝒃𝒍𝒆 implementation can be so faster than 𝒔𝒕𝒓𝒖𝒄𝒕 or
𝒓𝒆𝒄𝒐𝒓𝒅 𝒔𝒕𝒓𝒖𝒄𝒕 implementation.
The 𝒔𝒕𝒓𝒖𝒄𝒕 equality implementation is in ValueType.Equals(Object) and relies on reflection (very slow, use memory allocation).
The 𝒓𝒆𝒄𝒐𝒓𝒅 𝒔𝒕𝒓𝒖𝒄𝒕 equality implementation is compiler synthesized and uses the declared data members (less coding, more readable, a little faster).


𝐌𝐨𝐫𝐞 𝐢𝐧𝐟𝐨𝐫𝐦𝐚𝐭𝐢𝐨𝐧: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record#value-equality