
public interface IDashboardService {
  List<DashboardEntity> FetchAnnouncements(); 
  DashboardResponse PostAnnouncement(DashboardDto dashboardDto);
  DashboardResponse DeleteAnnouncement(int annId);
}
