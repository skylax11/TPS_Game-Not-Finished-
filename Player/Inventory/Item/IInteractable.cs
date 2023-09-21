using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IInteractable
    {
       public void AddItem(SlotItemInfos[] All_Items, WeaponComponent _detectedItem,int id);

    }
}
