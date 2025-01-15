﻿using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeestBeest.Web.Controllers;

public class ProductController(ProductService productService, OrderService orderService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var productDtos = productService.GetProducts();
        var products = productDtos.Select(dto => new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Type = dto.Type,
            Img = dto.Img
        }).ToList();

        var productsOverviewModel = new ProductsOverViewModel { Products = products };
        return View(productsOverviewModel);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var model = new OneProductViewModel
        {
            AvailableImages = productService.GetAvailableImages(),
            AvailableTypes = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().ToList()
        };
        return View(model);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(OneProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            await HandleProductSave(productViewModel, true);
            return RedirectToAction("Index");
        }
        return View(productViewModel);
    }

    private OneProductViewModel GetProductViewModel(int id)
    {
        var product = productService.GetProductById(id);
        return new OneProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Img = product.Img,
            Orders = orderService.GetAllOrdersByProductId(id),
            AvailableImages = productService.GetAvailableImages(),
            AvailableTypes = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().ToList()
        };
    }

    [HttpGet("{id:int}/details")]
    public IActionResult Details(int id)
    {
        var productViewModel = GetProductViewModel(id);
        return View(productViewModel);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var productViewModel = GetProductViewModel(id);
        return View(productViewModel);
    }

    [HttpPost("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, OneProductViewModel productViewModel)
    {
        await HandleProductSave(productViewModel, false, id);
        return View(productViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await productService.DeleteBeestjeAsync(id);
        return Ok();
    }

    private async Task HandleProductSave(OneProductViewModel productViewModel, bool isCreate, int? id = null)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var productDto = productViewModel.ToDto();
                (bool check, string result) operationResult = isCreate ? productService.CreateProduct(productDto) : productService.UpdateProduct(id.Value, productDto);
                productViewModel.Check = operationResult.check;
                productViewModel.Result = operationResult.result;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
        }
    }
}