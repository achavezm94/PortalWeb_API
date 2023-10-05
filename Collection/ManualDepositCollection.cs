﻿using Microsoft.Data.SqlClient.Server;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Collection
{
    public class ManualDepositosCollection : List<OManual>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                new SqlMetaData("Usuarios_idFk", SqlDbType.VarChar, 50),
                new SqlMetaData("Machine_Sn", SqlDbType.VarChar, 50),
                new SqlMetaData("Transaccion_No", SqlDbType.Int),
                new SqlMetaData("FechaTransaccion", SqlDbType.DateTime),
                new SqlMetaData("DivisaTransaccion", SqlDbType.VarChar, 50),
                new SqlMetaData("Manual_Deposito_Bill_1", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_2", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_5", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_10", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_20", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_50", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Bill_100", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_1", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_5", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_10", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_25", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_50", SqlDbType.Int),
                new SqlMetaData("Manual_Deposito_Coin_100", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_1", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_2", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_5", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_10", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_20", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_50", SqlDbType.Int),
                new SqlMetaData("Total_Deposito_Bill_100", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_1", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_2", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_5", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_10", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_20", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_50", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Bill_100", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_1", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_5", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_10", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_25", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_50", SqlDbType.Int),
                new SqlMetaData("Total_Manual_Deposito_Coin_100", SqlDbType.Int),
                new SqlMetaData("Active", SqlDbType.VarChar, 1));
            foreach (OManual manual in this)
            {
                sqlRow.SetString(0, manual.User_id);
                sqlRow.SetString(1, manual.Machine_Sn);
                sqlRow.SetInt32(2, manual.Transaction_no);
                sqlRow.SetDateTime(3, manual.Time_generated);
                sqlRow.SetString(4, manual.Deposit_currency);
                sqlRow.SetInt32(5, manual.Deposit_denom_1);
                sqlRow.SetInt32(6, manual.Deposit_denom_2);
                sqlRow.SetInt32(7, manual.Deposit_denom_5);
                sqlRow.SetInt32(8, manual.Deposit_denom_10);
                sqlRow.SetInt32(9, manual.Deposit_denom_20);
                sqlRow.SetInt32(10, manual.Deposit_denom_50);
                sqlRow.SetInt32(11, manual.Deposit_denom_100);
                sqlRow.SetInt32(12, manual.Deposit_coin_1);
                sqlRow.SetInt32(13, manual.Deposit_coin_5);
                sqlRow.SetInt32(14, manual.Deposit_coin_10);
                sqlRow.SetInt32(15, manual.Deposit_coin_25);
                sqlRow.SetInt32(16, manual.Deposit_coin_50);
                sqlRow.SetInt32(17, manual.Deposit_coin_100);
                sqlRow.SetInt32(18, manual.Total_deposit_denom_1);
                sqlRow.SetInt32(19, manual.Total_deposit_denom_2);
                sqlRow.SetInt32(20, manual.Total_deposit_denom_5);
                sqlRow.SetInt32(21, manual.Total_deposit_denom_10);
                sqlRow.SetInt32(22, manual.Total_deposit_denom_20);
                sqlRow.SetInt32(23, manual.Total_deposit_denom_50);
                sqlRow.SetInt32(24, manual.Total_deposit_denom_100);
                sqlRow.SetInt32(25, manual.Total_manual_deposit_denom_1);
                sqlRow.SetInt32(26, manual.Total_manual_deposit_denom_2);
                sqlRow.SetInt32(27, manual.Total_manual_deposit_denom_5);
                sqlRow.SetInt32(28, manual.Total_manual_deposit_denom_10);
                sqlRow.SetInt32(29, manual.Total_manual_deposit_denom_20);
                sqlRow.SetInt32(30, manual.Total_manual_deposit_denom_50);
                sqlRow.SetInt32(31, manual.Total_manual_deposit_denom_100);
                sqlRow.SetInt32(32, manual.Total_manual_deposit_coin_1);
                sqlRow.SetInt32(33, manual.Total_manual_deposit_coin_5);
                sqlRow.SetInt32(34, manual.Total_manual_deposit_coin_10);
                sqlRow.SetInt32(35, manual.Total_manual_deposit_coin_25);
                sqlRow.SetInt32(36, manual.Total_manual_deposit_coin_50);
                sqlRow.SetInt32(37, manual.Total_manual_deposit_coin_100);
                sqlRow.SetString(38, manual.Active);
                yield return sqlRow;
            }
        }
    }
}
