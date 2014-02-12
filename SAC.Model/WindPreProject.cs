using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    public class WindPreProject
    {
        public int KeyId { get; set; }
        public string OrgId { get; set; }
        public string FDCGCMC { get; set; }
        public string LTPFWJ { get; set; }
        public string LTPFSJ { get; set; }
        public string FDCHZPFWJ { get; set; }
        public string FDCHZPFSJ { get; set; }
        public string JRXTPFWJ { get; set; }
        public string JRDWD { get; set; }
        public string JRDWDYDJ { get; set; }
        public string FDGHZJRL { get; set; }
        public string BQGCZJRL { get; set; }
        public string NPJFS { get; set; }
        public string NPJFGLMD { get; set; }
        public string SJLYXSS { get; set; }
        public string GCJTZTZ { get; set; }
        public string GCDTZTZ { get; set; }
        public string HPZFSWDJ { get; set; }
        public string CHECKSTATUS { get; set; }
        public string CHECKTIME { get; set; }
        public string CHECKUSER { get; set; }
        public string UPDATETIME { get; set; }
    }

    public class PreData
    {
        public int total { get; set; }
        public IList<WindPreProject> rows { get; set; }
        public string Msg { get; set; }
    }
}
