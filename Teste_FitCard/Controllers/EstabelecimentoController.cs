﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Teste_FitCard.Models;
using Teste_FitCard.Repository;

namespace Teste_FitCard.Controllers
{
    public class EstabelecimentoController : Controller
    {
        private Repository.EstabelecimentoRepository _repository = new EstabelecimentoRepository();

        // GET: Estabelecimento
        public ActionResult Index()
        {
            TempData["ESTABELICIMENTOLIST"] = _repository.GetAll();

            return View();
        }

        public ActionResult AddOrUpdate(string id)
        {
            CarregaCategoria();
            CarregaEstados();


            var model = new EstabelecimentoModel();
            if (!string.IsNullOrEmpty(id))
                model = _repository.GetAll().First(a => a.IdEstabelecimento == Convert.ToInt32(id));



            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(EstabelecimentoModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _repository.AddorUpdate(model);


                    ViewBag.sucesso = "Dados Salvo com sucesso";

                }


            }
            catch (Exception e)
            {
                ViewBag.error = $"Ocorreu um problema ao tentar gravar os dados {e.Message}";

            }

            CarregaCategoria();
            CarregaEstados();

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _repository.Delete(id);

            TempData["ESTABELICIMENTOLIST"] = _repository.GetAll();

            return View("Index");
        }

        private void CarregaCategoria()
        {
            var cat = new CategoriaRepository().GetCategoria();

            TempData["CategoriaList"] = new MultiSelectList(cat, "IdCategoria", "Categoria");
        }

        private void CarregaEstados()
        {
            var estados = new Servicos.IGBE_Service().GetEstados().OrderBy(a => a.nome).ToList();

            var states = new List<SelectListItem>();

            estados.ForEach(item =>
            {
                states.Add(new SelectListItem { Text = item.nome, Value = item.sigla });

            });

            ViewBag.States = states;

        }


        [HttpPost]
        public JsonResult GetCidade(string uf)
        {
            var cidades = new Servicos.IGBE_Service().GetCidades(uf);

            return Json(new SelectList(cidades, "nome", "nome"));
        }
    }
}