
using System.ComponentModel;

namespace Taime.Application.Enums
{
    public enum TaimeApiErrors
    {
        /// <summary>
        /// O id informado é inválido.
        /// </summary>
        [Description("O id informado é inválido.")]
        TaimeApi_Post_400_Invalid_Id,

        /// <summary>
        /// Informe email e senha para logar.
        /// </summary>
        [Description("Informe email e senha para logar.")]
        TaimeApi_Post_400_Invalid_Login,

        /// <summary>
        /// Usuário não encontrado.
        /// </summary>
        [Description("Usuário não encontrado.")]
        TaimeApi_Post_400_User_Not_Found,

        /// <summary>
        /// Já existe um usuário para o e-mail informado.
        /// </summary>
        [Description("Já existe um usuário para o e-mail informado.")]
        TaimeApi_Post_400_User_Already_Exists,

        /// <summary>
        /// Produto não encontrado.
        /// </summary>
        [Description("Produto não encontrado.")]
        TaimeApi_Post_400_Product_Not_Found,

        /// <summary>
        /// Marca não encontrada.
        /// </summary>
        [Description("Marca não encontrada.")]
        TaimeApi_Post_400_Brand_Not_Found,

        /// <summary>
        /// Categoria não encontrada.
        /// </summary>
        [Description("Categoria não encontrada.")]
        TaimeApi_Post_400_Category_Not_Found,

        /// <summary>
        /// Coleção não encontrada.
        /// </summary>
        [Description("Coleção não encontrada.")]
        TaimeApi_Post_400_Collection_Not_Found,

        /// <summary>
        /// Transação não encontrada.
        /// </summary>
        [Description("Transação não encontrada.")]
        TaimeApi_Post_400_Transaction_Not_Found,
    }
}