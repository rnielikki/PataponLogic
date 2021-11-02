using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Character.Patapon
{
    public class PataponGeneral : Patapon
    {
        private GeneralMode _generalMode { get; }
        public PataponGroup Group { get; }

        protected override Stat DefaultStat => throw new NotImplementedException();
    }
}
