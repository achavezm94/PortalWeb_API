namespace PortalWeb_API.Models
{
    public class ORecoleccion
    {
        public string? User_id { get; set; }
        public string? Machine_Sn { get; set; }
        public int Transaction_no { get; set; }
        public DateTime Time_generated { get; set; }
        public string? Collection_currency { get; set; }
        public int Total_deposit_denom_1 { get; set; }
        public int Total_deposit_denom_2 { get; set; }
        public int Total_deposit_denom_5 { get; set; }
        public int Total_deposit_denom_10 { get; set; }
        public int Total_deposit_denom_20 { get; set; }
        public int Total_deposit_denom_50 { get; set; }
        public int Total_deposit_denom_100 { get; set; }
        public int Total_manual_deposit_denom_1 { get; set; }
        public int Total_manual_deposit_denom_2 { get; set; }
        public int Total_manual_deposit_denom_5 { get; set; }
        public int Total_manual_deposit_denom_10 { get; set; }
        public int Total_manual_deposit_denom_20 { get; set; }
        public int Total_manual_deposit_denom_50 { get; set; }
        public int Total_manual_deposit_denom_100 { get; set; }
        public int Total_manual_deposit_coin_1 { get; set; }
        public int Total_manual_deposit_coin_5 { get; set; }
        public int Total_manual_deposit_coin_10 { get; set; }
        public int Total_manual_deposit_coin_25 { get; set; }
        public int Total_manual_deposit_coin_50 { get; set; }
        public int Total_manual_deposit_coin_100 { get; set; }
        public string? Active { get; set; }
    }
}
