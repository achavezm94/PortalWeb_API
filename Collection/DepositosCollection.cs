using Microsoft.Data.SqlClient.Server;
using PortalWeb_API.Models;
using System.Data;
namespace PortalWeb_API.Collection
{
    public class DepositosCollection : List<ODeposito>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                new SqlMetaData("Usuarios_idFk", SqlDbType.VarChar, 50),
                new SqlMetaData("Machine_Sn", SqlDbType.VarChar, 50),
                new SqlMetaData("Transaccion_No", SqlDbType.Int),
                new SqlMetaData("FechaTransaccion", SqlDbType.DateTime),
                new SqlMetaData("DivisaTransaccion", SqlDbType.VarChar, 50),
                new SqlMetaData("Deposito_Bill_1", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_2", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_5", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_10", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_20", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_50", SqlDbType.Int),
                new SqlMetaData("Deposito_Bill_100", SqlDbType.Int),
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
            foreach (ODeposito deposito in this)
            {
                sqlRow.SetString(0, deposito.User_id);
                sqlRow.SetString(1, deposito.Machine_Sn);
                sqlRow.SetInt32(2, deposito.Transaction_no);
                sqlRow.SetDateTime(3, deposito.Time_generated);
                sqlRow.SetString(4, deposito.Deposit_currency);
                sqlRow.SetInt32(5, deposito.Deposit_denom_1);
                sqlRow.SetInt32(6, deposito.Deposit_denom_2);
                sqlRow.SetInt32(7, deposito.Deposit_denom_5);
                sqlRow.SetInt32(8, deposito.Deposit_denom_10);
                sqlRow.SetInt32(9, deposito.Deposit_denom_20);
                sqlRow.SetInt32(10, deposito.Deposit_denom_50);
                sqlRow.SetInt32(11, deposito.Deposit_denom_100);
                sqlRow.SetInt32(12, deposito.Total_deposit_denom_1);
                sqlRow.SetInt32(13, deposito.Total_deposit_denom_2);
                sqlRow.SetInt32(14, deposito.Total_deposit_denom_5);
                sqlRow.SetInt32(15, deposito.Total_deposit_denom_10);
                sqlRow.SetInt32(16, deposito.Total_deposit_denom_20);
                sqlRow.SetInt32(17, deposito.Total_deposit_denom_50);
                sqlRow.SetInt32(18, deposito.Total_deposit_denom_100);
                sqlRow.SetInt32(19, deposito.Total_manual_deposit_denom_1);
                sqlRow.SetInt32(20, deposito.Total_manual_deposit_denom_2);
                sqlRow.SetInt32(21, deposito.Total_manual_deposit_denom_5);
                sqlRow.SetInt32(22, deposito.Total_manual_deposit_denom_10);
                sqlRow.SetInt32(23, deposito.Total_manual_deposit_denom_20);
                sqlRow.SetInt32(24, deposito.Total_manual_deposit_denom_50);
                sqlRow.SetInt32(25, deposito.Total_manual_deposit_denom_100);
                sqlRow.SetInt32(26, deposito.Total_manual_deposit_coin_1);
                sqlRow.SetInt32(27, deposito.Total_manual_deposit_coin_5);
                sqlRow.SetInt32(28, deposito.Total_manual_deposit_coin_10);
                sqlRow.SetInt32(29, deposito.Total_manual_deposit_coin_25);
                sqlRow.SetInt32(30, deposito.Total_manual_deposit_coin_50);
                sqlRow.SetInt32(31, deposito.Total_manual_deposit_coin_100);
                sqlRow.SetString(32, deposito.Active);
                yield return sqlRow;
            }
        }
    }
}
