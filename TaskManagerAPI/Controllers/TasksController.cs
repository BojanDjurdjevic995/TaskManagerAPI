using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    public class TasksController : ApiController
    {
        object data = new { };
        TasksEntities db = new TasksEntities();
        // GET: api/Tasks
        public IHttpActionResult Get()
        {
            var tasks = db.tasks.ToList();
            return Ok(tasks);
        }
            
        // GET: api/Tasks/5
        public IHttpActionResult Get(int id)
        {
            var tasks = db.tasks.Find(id);
            if (tasks == null)
            {
                data = new
                {
                    success = false,
                    message = "We can't find the task!"
                };
                return Content(HttpStatusCode.NotFound, data);
            }

            return Ok(tasks);
        }

        // POST: api/Tasks
        public IHttpActionResult Post(tasks task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.tasks.Add(task);
            db.SaveChanges();
            data = new
            {
                success = true,
                message = "Successfully Create Task!",
                task = task
            };
            return Ok(data);
        }

        // PUT: api/Tasks/5
        public IHttpActionResult Put(int id, tasks task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateTask = db.tasks.Find(id);
            if (updateTask == null)
            {
                data = new
                {
                    success = false,
                    message = "We can't find the task!"
                };
                return Content(HttpStatusCode.NotFound, data);
            }
                
            updateTask.name = task.name;
            updateTask.status = task.status;
            updateTask.priority = task.priority;
            updateTask.start_date = task.start_date;
            updateTask.end_date = task.end_date;
            updateTask.notes = task.notes;

            db.SaveChanges();
            data = new
            {
                success = true,
                message = "Successfully Update Task!"
            };
            return Ok(data);
        }

        // DELETE: api/Tasks/5
        public IHttpActionResult Delete(int id)
        {
            var deleteTask = db.tasks.Find(id);
            if (deleteTask == null)
            {
                data = new
                {
                    success = false,
                    message = "We can't find the task!"
                };
                return Content(HttpStatusCode.NotFound, data);
            }

            db.Entry(deleteTask).State = EntityState.Deleted;
            db.SaveChanges();
            data = new
            {
                success = true,
                message = "Successfully Deleted Task!"
            };
            return Ok(data);
        }
    }
}
