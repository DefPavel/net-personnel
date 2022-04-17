using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPersonel.ViewModels
{
    internal class AddPositionViewModel : BaseViewModel
    {
        private readonly Users? user;

        public AddPositionViewModel(Users? user)
        {
            this.user = user;
        }
    }
}
