using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebTest5.Models;

namespace WebTest5.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly dbpruebaContext _context;

        public UsuariosController(dbpruebaContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {


              return _context.Usuarios != null ? 
                          View(await _context.Usuarios.ToListAsync()) :
                          Problem("Entity set 'dbpruebaContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Idusuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idusuario,Nombre")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Idusuario,Nombre")] Usuario usuario)
        {
            if (id != usuario.Idusuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Idusuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Idusuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        private static string cadena = "Data Source=localhost;Initial Catalog=dbprueba;Integrated Security=True";
       



        public void OtrasAcciones()
        {

            using (SqlConnection micon = new SqlConnection(cadena)) {

                SqlCommand comando = new SqlCommand("UPDATE USUARIOS SET NOMBRE = @nombre WHERE IDUSUARIO = @codigo", micon);
                comando.Parameters.AddWithValue("nombre", "antonio");
                comando.Parameters.AddWithValue("codigo", "7");
                micon.Open();
                int filas = comando.ExecuteNonQuery();
                Console.WriteLine("registros insertados " + filas);

            }



        }

       public static Perfil perfil = new Perfil();

        public async Task<IActionResult> AccionBaseDatos()
        {
            string elid = "";
            using (SqlConnection con = new SqlConnection(cadena)) {
                SqlCommand cmd = new SqlCommand("Select IDUSUARIO, NOMBRE FROM USUARIOS",con);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
              
                while (rd.Read()) {
                  
                    Console.WriteLine("codigo" + rd["IDUSUARIO"].ToString());
                    Console.WriteLine("nombre" + rd["NOMBRE"].ToString());
                  
                    elid = rd["IDUSUARIO"].ToString();
                }
                rd.Close();



            }



                Console.WriteLine("entrando a accion base datos");
            ViewData["elid"] = elid;
            return View();

        }







        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'dbpruebaContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(decimal id)
        {
          return (_context.Usuarios?.Any(e => e.Idusuario == id)).GetValueOrDefault();
        }




        [HttpPost]
 
        public async Task<IActionResult> GuardarPerfil([Bind("Apellido,Fechahora")] Perfil perfil)
        {

            Console.WriteLine("el apellido escrito fue: "+perfil.Apellido);
            string fechaformat = perfil.Fechahora.Date.ToString("yyyyMMdd");
            Console.WriteLine("la fecha escrito fue: " + fechaformat );


            return RedirectToAction(nameof(Index));
        }
}
}
