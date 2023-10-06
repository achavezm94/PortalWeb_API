namespace PortalWeb_API.Models
{
    public class ORecoleccion
    {
        public string? User_id { get; set; }
        public string? Machine_Sn { get; set; }

        private int _transaction_no;
        public int Transaction_no
        {
            get
            {
                return _transaction_no;
            }
            set
            {
                _transaction_no = value;

            }
        }

        private DateTime _time_generated;
        public DateTime Time_generated
        {
            get
            {
                return _time_generated;
            }
            set
            {
                _time_generated = value;

            }
        }

        public string? Collection_currency { get; set; }

        private int _total_deposit_denom_1;
        public int Total_deposit_denom_1
        {
            get
            {
                return _total_deposit_denom_1;
            }
            set
            {
                _total_deposit_denom_1 = value;

            }
        }

        private int _total_deposit_denom_2;
        public int Total_deposit_denom_2
        {
            get
            {
                return _total_deposit_denom_2;
            }
            set
            {
                _total_deposit_denom_2 = value;

            }
        }

        private int _total_deposit_denom_5;
        public int Total_deposit_denom_5
        {
            get
            {
                return _total_deposit_denom_5;
            }
            set
            {
                _total_deposit_denom_5 = value;

            }
        }

        private int _total_deposit_denom_10;
        public int Total_deposit_denom_10
        {
            get
            {
                return _total_deposit_denom_10;
            }
            set
            {
                _total_deposit_denom_10 = value;

            }
        }

        private int _total_deposit_denom_20;
        public int Total_deposit_denom_20
        {
            get
            {
                return _total_deposit_denom_20;
            }
            set
            {
                _total_deposit_denom_20 = value;

            }
        }

        private int _total_deposit_denom_50;
        public int Total_deposit_denom_50
        {
            get
            {
                return _total_deposit_denom_50;
            }
            set
            {
                _total_deposit_denom_50 = value;

            }
        }

        private int _total_deposit_denom_100;
        public int Total_deposit_denom_100
        {
            get
            {
                return _total_deposit_denom_100;
            }
            set
            {
                _total_deposit_denom_100 = value;

            }
        }

        private int _total_manual_deposit_denom_1;
        public int Total_manual_deposit_denom_1
        {
            get
            {
                return _total_manual_deposit_denom_1;
            }
            set
            {
                _total_manual_deposit_denom_1 = value;

            }
        }

        private int _total_manual_deposit_denom_2;
        public int Total_manual_deposit_denom_2
        {
            get
            {
                return _total_manual_deposit_denom_2;
            }
            set
            {
                _total_manual_deposit_denom_2 = value;

            }
        }

        private int _total_manual_deposit_denom_5;
        public int Total_manual_deposit_denom_5
        {
            get
            {
                return _total_manual_deposit_denom_5;
            }
            set
            {
                _total_manual_deposit_denom_5 = value;

            }
        }

        private int _total_manual_deposit_denom_10;
        public int Total_manual_deposit_denom_10
        {
            get
            {
                return _total_manual_deposit_denom_10;
            }
            set
            {
                _total_manual_deposit_denom_10 = value;

            }
        }

        private int _total_manual_deposit_denom_20;
        public int Total_manual_deposit_denom_20
        {
            get
            {
                return _total_manual_deposit_denom_20;
            }
            set
            {
                _total_manual_deposit_denom_20 = value;

            }
        }

        private int _total_manual_deposit_denom_50;
        public int Total_manual_deposit_denom_50
        {
            get
            {
                return _total_manual_deposit_denom_50;
            }
            set
            {
                _total_manual_deposit_denom_50 = value;

            }
        }

        private int _total_manual_deposit_denom_100;
        public int Total_manual_deposit_denom_100
        {
            get
            {
                return _total_manual_deposit_denom_100;
            }
            set
            {
                _total_manual_deposit_denom_100 = value;

            }
        }

        private int _total_manual_deposit_coin_1;
        public int Total_manual_deposit_coin_1
        {
            get
            {
                return _total_manual_deposit_coin_1;
            }
            set
            {
                _total_manual_deposit_coin_1 = value;

            }
        }

        private int _total_manual_deposit_coin_5;
        public int Total_manual_deposit_coin_5
        {
            get
            {
                return _total_manual_deposit_coin_5;
            }
            set
            {
                _total_manual_deposit_coin_5 = value;

            }
        }

        private int _total_manual_deposit_coin_10;
        public int Total_manual_deposit_coin_10
        {
            get
            {
                return _total_manual_deposit_coin_10;
            }
            set
            {
                _total_manual_deposit_coin_10 = value;

            }
        }

        private int _total_manual_deposit_coin_25;
        public int Total_manual_deposit_coin_25
        {
            get
            {
                return _total_manual_deposit_coin_25;
            }
            set
            {
                _total_manual_deposit_coin_25 = value;

            }
        }

        private int _total_manual_deposit_coin_50;
        public int Total_manual_deposit_coin_50
        {
            get
            {
                return _total_manual_deposit_coin_50;
            }
            set
            {
                _total_manual_deposit_coin_50 = value;

            }
        }

        private int _total_manual_deposit_coin_100;
        public int Total_manual_deposit_coin_100
        {
            get
            {
                return _total_manual_deposit_coin_100;
            }
            set
            {
                _total_manual_deposit_coin_100 = value;

            }
        }

        public string? Active { get; set; }
    }
}
