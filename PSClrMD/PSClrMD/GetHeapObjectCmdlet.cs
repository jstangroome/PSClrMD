using System.Management.Automation;
using Microsoft.Diagnostics.Runtime;

namespace PSClrMD
{
    [Cmdlet(VerbsCommon.Get, Context.DefaultCommandPrefix + "HeapObject")]
    [OutputType(typeof(ClrHeapObject))]
    public class GetHeapObjectCmdlet : ClrMDRuntimeCmdlet
    {
        [Parameter]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            var runtime = GetRuntime();

            var heap = runtime.GetHeap();
            if (!heap.CanWalkHeap)
            {
                if (!Force.IsPresent)
                {
                    throw new PSClrMDException("Heap is not walkable. Specify Force to override.");
                }
                WriteWarning("Heap is not walkable. Enumerating anyway.");
            }

            foreach (var objectAddress in heap.EnumerateObjects())
            {
                var objectType = heap.GetObjectType(objectAddress);
                var objectSize = objectType.GetSize(objectAddress);
                var objectGeneration = heap.GetGeneration(objectAddress);

                ClrException exceptionValue = null;
                if (objectType.IsException)
                {
                    exceptionValue = heap.GetExceptionObject(objectAddress);
                }

                object simpleValue = null;
                if (objectType.HasSimpleValue)
                {
                    simpleValue = objectType.GetValue(objectAddress);
                }

                WriteObject(new ClrHeapObject(objectAddress, objectType, objectSize, objectGeneration, simpleValue, exceptionValue));
            }

            WriteObject(runtime.Threads, enumerateCollection: true);
        }
    }
}