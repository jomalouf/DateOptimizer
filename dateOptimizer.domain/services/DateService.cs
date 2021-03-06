using System;
using System.Threading.Tasks;
using dateOptimizer.domain.contracts;
using dateOptimizer.domain.DataTransferObjects;

namespace dateOptimizer.domain.services
{
    public class DateService : IDateService
    {
        private IRepository _repository;

        public DateService(IRepository repository)
        {
            this._repository = repository ?? throw new ArgumentNullException();
        }

        public async Task<DayRangeDto> GetDatesByCountyAsync(CountyDto county)
        {
            FipInformationDto fipInfo = await _repository.GetDayRangeByCountyAsync(county).ConfigureAwait(false);
            
            int start = -1;
            int end = -1;
            for (int i = 0; i < fipInfo.Days.Length; i++) {
                if (fipInfo.Days[i] > 0 && start == -1) {
                    start = i;
                }
                if (fipInfo.Days[i] > .75 && end == -1) {
                    end = i;
                }
            }

            int range = end - start;

            var dayRange = new DayRangeDto() 
            {
                StartDay = start,
                DayRange = range,
            };


            return dayRange;
        }

        public async Task<DayRangeDto> GetDatesByFipAsync(int fip)
        {

            FipInformationDto fipInfo = await _repository.GetDayRangeByFipAsync(fip).ConfigureAwait(false);
            
            int start = -1;
            int end = -1;
            for (int i = 0; i < fipInfo.Days.Length; i++) {
                if (fipInfo.Days[i] > 0 && start == -1) {
                    start = i;
                }
                if (fipInfo.Days[i] > .75 && end == -1) {
                    end = i;
                }
            }

            int range = end - start;

            var dayRange = new DayRangeDto() 
            {
                StartDay = start,
                DayRange = range,
            };


            return dayRange;
        }
    }
}