using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Rental {
    public class UC_RentalMovies_UIT : UC_UIT {
        

        public UC_RentalMovies_UIT(ITestOutputHelper output) : base(output) {
        }

        private void Precondition_perform_login() {
            Perform_login("elena@uclm.es", "Password1234%");
        }


    }
}
