namespace LiaSoft.Jeff.DataAccess
{
    /// <summary>
    /// Интерфейс для приведжения значения к значению DbParameter
    /// </summary>
    public interface IDbParameterValue
    {
        object GetParameterValue();
    }
}
