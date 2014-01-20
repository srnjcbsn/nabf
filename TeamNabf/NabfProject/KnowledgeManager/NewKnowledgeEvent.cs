﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NabfProject.KnowledgeManager
{
    public class NewKnowledgeEvent : XmasEngineModel.Management.XmasEvent
    {
        public Knowledge NewKnowledge;


        public NewKnowledgeEvent(Knowledge k)
        {
            NewKnowledge = k;
        }
    }
}
