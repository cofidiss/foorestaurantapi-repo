using FooRestaurantApi.DomainLayer.Entity.TaskTakip;
using FooRestaurantApi.DomainLayer.Model.TaskTakip;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations
{
    public class TaskTakipRepository : Repository<TaskEntity>, ITaskTakipRepository
    {
        PostgreDbContext _dbContext { get; }

        public TaskTakipRepository(PostgreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public TaskTakipMainModel GetTaskResult(int weekIdStart, int weekIdEnd)
        {

            var taskTakipMainModel = new TaskTakipMainModel();


            int[] weekIdArr = Enumerable.Range(weekIdStart, weekIdEnd).ToArray();
            var tableResultAsStringModelList = new List<TableResultAsStringModel>();
            taskTakipMainModel.TableResultAsStringModelList = tableResultAsStringModelList;
            var categoryList = (from category in _dbContext.Set<CategoryEntity>()
                                  
                                  select category).ToList();
            taskTakipMainModel.CategoryEntityList = categoryList;
            Func<int, List<TaskEntity>, string> taskForCategory = delegate (int categoryId,List<TaskEntity> taskLisForWeek)
            {
                var category1ResultStr = String.Empty;
                var taskForCategories = taskLisForWeek.Where(x => x.CategoryId == categoryId);

                if (taskForCategories.Count() ==0)
                {
                    return category1ResultStr;


                }
                 foreach (var task in taskForCategories)
                {
                    category1ResultStr += $"-{task.Description} \n {task.EstimatedWorkHour}h / {task.WorkedHour}h \n {task.PercentCompletion}% \n";




                }
                category1ResultStr += $" planlanan Saat/ Çalışılan Saat: {taskForCategories.Sum(x => x.EstimatedWorkHour)}h /  {taskForCategories.Sum(x => x.WorkedHour)}h  Tamamlanma Yüzdesi:{taskForCategories.Average(x => x.PercentCompletion) * 100}%";
                return category1ResultStr;
            };


            foreach (int weekId in weekIdArr)
            {
                var tableResultAsStringModel = new TableResultAsStringModel();
                tableResultAsStringModel.WeekId = weekId;

                var taskLisForWeek = (from task in _dbContext.Set<TaskEntity>()
                                      where task.WeekId == weekId
                                      select task).ToList();
                if (taskLisForWeek.Count() ==0)
                {
                    continue;


                }
       
                tableResultAsStringModel.Category1Result =   taskForCategory(1, taskLisForWeek);
                tableResultAsStringModel.Category2Result = taskForCategory(2, taskLisForWeek);
                tableResultAsStringModel.Category3Result = taskForCategory(3, taskLisForWeek);
                tableResultAsStringModel.Category4Result = taskForCategory(4, taskLisForWeek);
                tableResultAsStringModel.Category5Result = taskForCategory(5, taskLisForWeek);
                tableResultAsStringModel.Category6Result = taskForCategory(6, taskLisForWeek);
                tableResultAsStringModel.Category7Result = taskForCategory(7, taskLisForWeek);
                tableResultAsStringModel.Category8Result = taskForCategory(8, taskLisForWeek);
                tableResultAsStringModel.TotalEstimatedWorkHour = taskLisForWeek.Sum(x => x.EstimatedWorkHour).ToString();

                tableResultAsStringModel.TotalWorkedHour = taskLisForWeek.Sum(x => x.WorkedHour).ToString();
                tableResultAsStringModel.TotalPercentCompletion = (taskLisForWeek.Average(x => x.PercentCompletion) *100).ToString();
                tableResultAsStringModelList.Add(tableResultAsStringModel);
            }

            return taskTakipMainModel;
        }
    }
}
