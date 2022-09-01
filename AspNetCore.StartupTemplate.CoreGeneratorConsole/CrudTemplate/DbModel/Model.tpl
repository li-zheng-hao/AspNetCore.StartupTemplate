using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace {{NameSpacePrefix}}.Model
{
    /// <summary>
    /// {{ModelDescription}}
    /// </summary>
    public class {{ModelClassName}}
    {
		{% for field in ModelFields %}
        /// <summary>
        /// {{field.ColumnDescription}}
        /// </summary>
        [Display(Name = "{{field.ColumnDescription}}")]
		{% if field.IsPrimarykey == true %}
        [Column(IsPrimaryKey = true)]
        {% else %}{% endif %}
        {% if field.IsNullable == false %}[Required(ErrorMessage = "请输入{0}")]{% endif %}
        {% if field.DataType == 'nvarchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'varchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'nvarchar' or field.DataType == 'varchar'  or field.DataType == 'text' %}
        public System.String {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == false  %}
        public System.Int32 {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == true %}
        public System.Int32? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == false  %}
        public System.Int64 {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == true %}
        public System.Int64? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == false  %}
        public float {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == true %}
        public float? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == false %}
        public System.Boolean {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == true %}
        public System.Boolean? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == false %}
        public System.Guid {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == true %}
        public System.Guid? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% else %}
        {% endif %}
		{% endfor %}
    }
}
