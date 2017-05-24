using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SK;
using SKvisual;

namespace SKTest
{
    [TestFixture]
    public class SK_CreateRandomTest
    {
        [Ignore("stackoverflow error")]
        [Test]
        public void GenerateSKTest()
        {
            
            
            SKCreator.CreateRandomMattrixShuffle(SKCreator.GenerateSeedMattrix(),4);


            var skr = SKCreator.CreateRandomMattrixRec(new SKMattrix(Enumerable.Empty<SKSingle>()));
        }
        
    }
}
