using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_List
{
    public record TodoItem
    (
        string Description,    
        DateTime Deadline,     
        int Priority,          
        string Category        
    );
}
