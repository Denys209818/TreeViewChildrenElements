using System;
using System.Collections.Generic;
using System.Text;

namespace TreeViewAddElement.Models
{
    public class ComboBoxModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
