﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectedItemsContractLib
{
    public interface ISelectedItemsContract
    {
        List<string> GetAllSelectedItems(int Feature_No, string FeatureValue);
    }
}
