using Microsoft.Data.SqlClient.Server;
using PortalWeb_API.Models;
using System.Data;

namespace PortalWeb_API.Collection
{
    public class RecoleccionCollection : List<ORecoleccion>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                new SqlMetaData("Usuarios_idFk", SqlDbType.VarChar, 50),
                new SqlMetaData("Machine_Sn", SqlDbType.VarChar, 50),
                new SqlMetaData("Transaccion_No", SqlDbType.Int),
                new SqlMetaData("FechaTransaccion", SqlDbType.DateTime),
                new SqlMetaData("DivisaTransaccion", SqlDbType.VarChar, 50),
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
            foreach (ORecoleccion recoleccion in this)
            {
                sqlRow.SetString(0, recoleccion.User_id);
                sqlRow.SetString(1, recoleccion.Machine_Sn);
                sqlRow.SetInt32(2, recoleccion.Transaction_no);
                sqlRow.SetDateTime(3, recoleccion.Time_generated);
                sqlRow.SetString(4, recoleccion.Collection_currency);
                sqlRow.SetInt32(5, recoleccion.Total_deposit_denom_1);
                sqlRow.SetInt32(6, recoleccion.Total_deposit_denom_2);
                sqlRow.SetInt32(7, recoleccion.Total_deposit_denom_5);
                sqlRow.SetInt32(8, recoleccion.Total_deposit_denom_10);
                sqlRow.SetInt32(9, recoleccion.Total_deposit_denom_20);
                sqlRow.SetInt32(10, recoleccion.Total_deposit_denom_50);
                sqlRow.SetInt32(11, recoleccion.Total_deposit_denom_100);
                sqlRow.SetInt32(12, recoleccion.Total_manual_deposit_denom_1);
                sqlRow.SetInt32(13, recoleccion.Total_manual_deposit_denom_2);
                sqlRow.SetInt32(14, recoleccion.Total_manual_deposit_denom_5);
                sqlRow.SetInt32(15, recoleccion.Total_manual_deposit_denom_10);
                sqlRow.SetInt32(16, recoleccion.Total_manual_deposit_denom_20);
                sqlRow.SetInt32(17, recoleccion.Total_manual_deposit_denom_50);
                sqlRow.SetInt32(18, recoleccion.Total_manual_deposit_denom_100);
                sqlRow.SetInt32(19, recoleccion.Total_manual_deposit_coin_1);
                sqlRow.SetInt32(20, recoleccion.Total_manual_deposit_coin_5);
                sqlRow.SetInt32(21, recoleccion.Total_manual_deposit_coin_10);
                sqlRow.SetInt32(22, recoleccion.Total_manual_deposit_coin_25);
                sqlRow.SetInt32(23, recoleccion.Total_manual_deposit_coin_50);
                sqlRow.SetInt32(24, recoleccion.Total_manual_deposit_coin_100);
                sqlRow.SetString(25, recoleccion.Active);
                yield return sqlRow;
            }
        }
    }
}
