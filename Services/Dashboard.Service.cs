public class DashboardService: IDashboardService
{
    private readonly SMSDbContext context;
    public DashboardService(SMSDbContext context)
    {
        this.context = context;
    }

    public List<DashboardEntity> FetchAnnouncements()
    {
        List<DashboardEntity> dashboard =  context.Dashboard.ToList();
        dashboard.Sort((x,y) => y.Id.CompareTo(x.Id));
        return dashboard;
    }

    public DashboardResponse PostAnnouncement(DashboardDto dashboardDto)
    {
        if (string.IsNullOrEmpty(dashboardDto.Title) || string.IsNullOrWhiteSpace(dashboardDto.Title))
        {
            return new DashboardResponse
            {
                Success = false,
                Message = "Title is required"
            };
        }


        if (string.IsNullOrEmpty(dashboardDto.Message) || string.IsNullOrWhiteSpace(dashboardDto.Message))
        {
            return new DashboardResponse
            {
                Success = false,
                Message = "Message is required"
            };
        }

        DashboardEntity newDashboard = new DashboardEntity()
        {
          Title = dashboardDto.Title,
          Message = dashboardDto.Message,
          Date = DateTime.Now.ToString("MMMM dd yyyy") 
        };

        context.Dashboard.Add(newDashboard);
        context.SaveChanges();

        return new DashboardResponse {
          Success = true,
          Message = "Dashboard Updated",
          Dashboard = newDashboard
        };
    }

    public DashboardResponse DeleteAnnouncement(int annId)
    {
      var announcement = context.Dashboard.Find(annId);
      if(announcement == null)
      {
        return new DashboardResponse {
          Success = false,
          Message = "Announcement id not found in the database"
        };
      };

      context.Dashboard.Remove(announcement);
      return new DashboardResponse {
        Success = true,
        Message = "Post removed successfully !"
      };
    }
    
}
