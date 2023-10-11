﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalWeb_API.Models
{
    public partial class ManualDepositPeso
    {
            [Column("id")]
            public int Id { get; set; }

            [Required]
            [Column("Usuarios_idFk")]
            [StringLength(50)]
            [Unicode(false)]
            public string UsuariosIdFk { get; set; }

            [Column("Machine_Sn")]
            [StringLength(50)]
            [Unicode(false)]
            public string MachineSn { get; set; }

            [Column("Transaccion_No")]
            public int TransaccionNo { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime FechaTransaccion { get; set; }

            [Required]
            [StringLength(50)]
            [Unicode(false)]
            public string DivisaTransaccion { get; set; }

            [Column("Manual_Deposito_Bill_1")]
            public int? ManualDepositoBill1 { get; set; }

            [Column("Manual_Deposito_Bill_2")]
            public int? ManualDepositoBill2 { get; set; }

            [Column("Manual_Deposito_Bill_5")]
            public int? ManualDepositoBill5 { get; set; }

            [Column("Manual_Deposito_Bill_10")]
            public int? ManualDepositoBill10 { get; set; }

            [Column("Manual_Deposito_Bill_20")]
            public int? ManualDepositoBill20 { get; set; }

            [Column("Manual_Deposito_Bill_50")]
            public int? ManualDepositoBill50 { get; set; }

            [Column("Manual_Deposito_Bill_100")]
            public int? ManualDepositoBill100 { get; set; }

            [Column("Manual_Deposito_Coin_1")]
            public int? ManualDepositoCoin1 { get; set; }

            [Column("Manual_Deposito_Coin_5")]
            public int? ManualDepositoCoin5 { get; set; }

            [Column("Manual_Deposito_Coin_10")]
            public int? ManualDepositoCoin10 { get; set; }

            [Column("Manual_Deposito_Coin_25")]
            public int? ManualDepositoCoin25 { get; set; }

            [Column("Manual_Deposito_Coin_50")]
            public int? ManualDepositoCoin50 { get; set; }

            [Column("Manual_Deposito_Coin_100")]
            public int? ManualDepositoCoin100 { get; set; }

            [Column("Total_Deposito_Bill_1")]
            public int? TotalDepositoBill1 { get; set; }

            [Column("Total_Deposito_Bill_2")]
            public int? TotalDepositoBill2 { get; set; }

            [Column("Total_Deposito_Bill_5")]
            public int? TotalDepositoBill5 { get; set; }

            [Column("Total_Deposito_Bill_10")]
            public int? TotalDepositoBill10 { get; set; }

            [Column("Total_Deposito_Bill_20")]
            public int? TotalDepositoBill20 { get; set; }

            [Column("Total_Deposito_Bill_50")]
            public int? TotalDepositoBill50 { get; set; }

            [Column("Total_Deposito_Bill_100")]
            public int? TotalDepositoBill100 { get; set; }

            [Column("Total_Manual_Deposito_Bill_1")]
            public int? TotalManualDepositoBill1 { get; set; }

            [Column("Total_Manual_Deposito_Bill_2")]
            public int? TotalManualDepositoBill2 { get; set; }

            [Column("Total_Manual_Deposito_Bill_5")]
            public int? TotalManualDepositoBill5 { get; set; }

            [Column("Total_Manual_Deposito_Bill_10")]
            public int? TotalManualDepositoBill10 { get; set; }

            [Column("Total_Manual_Deposito_Bill_20")]
            public int? TotalManualDepositoBill20 { get; set; }

            [Column("Total_Manual_Deposito_Bill_50")]
            public int? TotalManualDepositoBill50 { get; set; }

            [Column("Total_Manual_Deposito_Bill_100")]
            public int? TotalManualDepositoBill100 { get; set; }

            [Column("Total_Manual_Deposito_Coin_1")]
            public int? TotalManualDepositoCoin1 { get; set; }

            [Column("Total_Manual_Deposito_Coin_5")]
            public int? TotalManualDepositoCoin5 { get; set; }

            [Column("Total_Manual_Deposito_Coin_10")]
            public int? TotalManualDepositoCoin10 { get; set; }

            [Column("Total_Manual_Deposito_Coin_25")]
            public int? TotalManualDepositoCoin25 { get; set; }

            [Column("Total_Manual_Deposito_Coin_50")]
            public int? TotalManualDepositoCoin50 { get; set; }

            [Column("Total_Manual_Deposito_Coin_100")]
            public int? TotalManualDepositoCoin100 { get; set; }

            [Required]
            [StringLength(1)]
            [Unicode(false)]
            public string Active { get; set; }

            [Column("Total_Manual_Deposito_Coin_100")]
            public double? Peso { get; set; }
        }
}
