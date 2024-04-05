﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuintoInventario.Models;

namespace QuintoInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly InventarioContext _context;

        public ProductosController(InventarioContext context)
        {
            _context = context;
        }

        // GET: api/Productos
   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _context.Productos
                            .Join(
                                _context.CategoriaProductos, // Tabla de categoría
                                p => p.IdCategoria,          // Clave foránea en Producto
                                cp => cp.Id,                 // Clave primaria en CategoriaProducto
                                (p, cp) => new Producto
                                {
                                    Id = p.Id,
                                    Nombre = p.Nombre,
                                    IdCategoria = p.IdCategoria,
                                    IdCategoriaNavigation = new CategoriaProducto
                                    {
                                        Id = cp.Id,
                                        Nombre = cp.Nombre,
                                     
                                    }
                                }
                            )
                            .ToListAsync();

            return productos;
        }



        [HttpGet("categoria")]
        public async Task<ActionResult<IEnumerable<CategoriaProducto>>> GetCategorias()
        {
            return await _context.CategoriaProductos.ToListAsync();
        }



        // GET: api/Productos/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Producto>> GetProducto(int id)

        {
            var producto = await _context.Productos
                            .Where(p => p.Id == id)
                            .Join(
                                _context.CategoriaProductos, // Tabla de categoría
                                p => p.IdCategoria,          // Clave foránea en Producto
                                cp => cp.Id,                 // Clave primaria en CategoriaProducto
                                (p, cp) => new Producto
                                {
                                    Id = p.Id,
                                    Nombre = p.Nombre,
                                    IdCategoria = p.IdCategoria,
                                    IdCategoriaNavigation = new CategoriaProducto
                                    {
                                        Id = cp.Id,
                                        Nombre = cp.Nombre
                                    }
                                }
                            )
                            .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }



        // PUT: api/Productoes/5
   
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productoes
    
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        // DELETE: api/Productoes/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

     
        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
