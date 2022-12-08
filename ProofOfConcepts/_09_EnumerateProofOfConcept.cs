using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.ProofOfConcepts;

public class EnumerateProofOfConcept: ProofOfConcept
{
    public override int Number => 9;
    public override void run(string[] args)
    {
        const int count = 10;
        var GuidsFor = new List<Guid>(count);
        for (int i = 1; i < count; i++)
        {
            GuidsFor.Add(Guid.NewGuid());
        }

        var GuidsEnumerable = Enumerable.Range(1, count).Select(x => Guid.NewGuid());

        var GuidsEnumerableList = Enumerable.Range(1, count).Select(x => Guid.NewGuid()).ToList();

        var GuidsFor1 = GuidsFor.ToList();
        var GuidsFor2 = GuidsFor.ToList();

        var GuidsEnumerable1 = GuidsEnumerable.ToList();
        var GuidsEnumerable2 = GuidsEnumerable.ToList();

        var GuidsEnumerableList1 = GuidsEnumerableList.ToList();
        var GuidsEnumerableList2 = GuidsEnumerableList.ToList();

        Console.WriteLine($"GuidsFor equality: {IsEqual(GuidsFor1, GuidsFor2)}");
        Console.WriteLine($"GuidsEnumerable equality: {IsEqual(GuidsEnumerable1, GuidsEnumerable2)}");
        Console.WriteLine($"GuidsEnumerableList equality: {IsEqual(GuidsEnumerableList1, GuidsEnumerableList2)}");
    }

    private bool IsEqual(List<Guid> aList, List<Guid> bList)
    {
        if (aList.Count != bList.Count)
        {
            return false;
        }

        for (int i = 0; i < aList.Count; i++)
        {
            if (aList[i] != bList[i])
                return false;
        }

        return true;
    }
}
