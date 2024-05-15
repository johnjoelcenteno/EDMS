namespace DPWH.EDMS.Application.Features.Agencies.Commands.BatchCreateAgencies;

public record Department(string NumberCode, string DepartmentCode, string Name);

public static class Departments
{
    public static IReadOnlyList<Department> List() => new List<Department>
    {
        new ("01", "OP", "Office of the President (OP)"),
        new ("02", "HOR&S", "Congress of the Philippines"),
        new ("03", "JUD", "The Judiciary"),
        new ("04", "CO", "Constitutional Offices"),
        new ("05", "DAR", "Department of Agrarian Reform (DAR)"),
        new ("06", "DAR", "Department of Agriculture (DA)"),
        new ("07", "DBM", "Department of Budget and Management (DBM)"),
        new ("08", "DepEd", "Department of Education (DepEd)"),
        new ("09", "DOE", "Department of Energy (DOE)"),
        new ("10", "DENR", "Department of Environment and Natural Resources (DENR)"),
        new ("11", "DOF", "Department of Finance (DOF)"),
        new ("12", "DFA", "Department of Foreign Affairs (DFA)"),
        new ("13", "DOH", "Department of Health (DOH)"),
        new ("14", "DILG", "Department of the Interior and Local Government (DILG)"),
        new ("15", "DOJ", "Department of Justice (DOJ)"),
        new ("16", "DOLE", "Department of Labor and Employment (DOLE)"),
        new ("17", "DND", "Department of National Defense (DND)"),
        new ("18", "DPWH", "Department of Public Works and Highways (DPWH)"),
        new ("19", "DOST", "Department of Science and Technology (DOST)"),
        new ("20", "DSWD", "Department of Social Welfare and Development (DSWD)"),
        new ("21", "DOT", "Department of Tourism (DOT)"),
        new ("22", "DTI", "Department of Trade and Industry (DTI)"),
        new ("23", "DoTr", "Department of Transportation (DoTr)"),
        new ("24", "NEDA", "National Economic and Development Authority (NEDA)"),
        new ("25", "DICT", "Department of Information and Communications Technology (DICT)"),
        new ("26", "DHSUD", "Department of Human Settlements and Urban Development (DHSUD)"),
        new ("27", "DMW", "Department of Migrant Workers (DMW)"),
        new ("28", "OEOs", "Other Executive Offices")
    };
}