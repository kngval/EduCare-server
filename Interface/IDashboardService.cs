
public interface IDashboardService {
  List<DashboardEntity> FetchAnnouncements(); 
  DashboardResponse PostAnnouncement(DashboardDto dashboardDto);
}
