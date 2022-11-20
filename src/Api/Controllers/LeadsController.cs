﻿using AutoMapper;
using Domain.Aggregates.Leads.ValueObjects;
using Domain.SharedKernel;
using Framework.Results;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Net;
using ViewModels.Lead;

namespace Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LeadsController : Infrustructure.ControllerBase
{
	private readonly IMapper mapper;

	public LeadsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
	{
		this.mapper = mapper;
	}

	[HttpGet]
	//[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	//[ProducesResponseType((int)HttpStatusCode.NotFound)]
	//[ProducesResponseType(typeof(Result<List<Domain.Aggregates.Leads.Lead>>),(int)HttpStatusCode.OK)]
	public async
		Task<Result<List<Domain.Aggregates.Leads.Lead>>> 
		GetLeads()
	{
		//var Result = new Framework.Results.Result<List<Domain.Aggregates.Leads.Lead>>();

		var leadModel = (await UnitOfWork.LeadRepository.GetAllAsync()).ToList();
		if (leadModel is null)
		{
			return new Result<List<Domain.Aggregates.Leads.Lead>>();
		}
		//var mappedModel = mapper.Map<ViewModels.Lead.LeadsViewModel>(leadModel);	

		var leadresult = leadModel.ToResult();

		return leadresult;
	}


	[HttpGet("{Id}")]
	public async
		Task<ActionResult<Result<Domain.Aggregates.Leads.Lead>>>
		GetLead(Guid Id)
	{
		var leadModel = await UnitOfWork.LeadRepository.GetByIdAsync(Id);
		if (leadModel is null)
		{
			return NoContent();
		}

		return Ok(leadModel.ToResult());

	}


	[HttpPost]
	public async Task<IActionResult> CreateLead(CreateLeadViewModel model)
	{
		var result = new Result<Domain.Aggregates.Leads.Lead>();
		try
		{
			var createLead = new
					Domain.Aggregates.Leads.Lead
					(model.TenentId
					, Salutation.GetByValue(model.Salutation)
					, FirstName.Create(model.FirstName)
					, LastName.Create(model.LastName)
					, EmailAddress.Create(model.Email)
					, LeadStatus.GetByValue(model.LeadStatus)
					, model.Title
					, model.Company
					, model.Mobile
					, model.Phone
					, Rating.GetByValue(model.Rating)
					, model.Country
					, model.State
					, model.City
					, model.Street
					, Industry.GetByValue(model.Industry)
					, model.AnnualRevenue
					, LeadSource.GetByValue(model.LeadSource)
					, model.PostalCode
					, model.NumberOfEmployees
					, model.Website
					, model.Description);

			await UnitOfWork.LeadRepository.AddAsync(createLead);

			await UnitOfWork.SaveAsync();

			result.WithData(createLead);

			return Created("http://test.com", result);

		}
		catch (Exception ex)
		{
			result.AddErrorMessage(ex.Message);
			return BadRequest(result);
		}


	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteLead(Guid Id)
	{
		var result = new Result<Domain.Aggregates.Leads.Lead>();

		try
		{
			var res = await UnitOfWork.LeadRepository.RemoveByIdAsync(Id);
			if (res == false)
			{
				result.AddErrorMessage(string.Format(Resources.Messages.Validations.NotFound,
								Resources.DataDictionary.Lead));
				return NotFound(result);
			}
			await UnitOfWork.SaveAsync();
			return Ok();
		}
		catch (Exception ex)
		{

			result.AddErrorMessage(ex.Message);
			return BadRequest(result);
		}
	}

	[HttpPatch]
	public async Task<IActionResult> UpdateLead(UpdateLeadViewModel model)
	{
		var result = new Result<Domain.Aggregates.Leads.Lead>();

		var leadModel = await UnitOfWork.LeadRepository.GetByIdAsync(model.Id);

		if (leadModel is null)
		{
			result.AddErrorMessage(string.Format(Resources.Messages.Validations.NotFound,
							Resources.DataDictionary.Lead));
			return NotFound(result);
		}

		try
		{
			leadModel.Update(
			 Salutation.GetByValue(model.Salutation)
			, FirstName.Create(model.FirstName)
			, LastName.Create(model.LastName)
			, EmailAddress.Create(model.Email)
			, LeadStatus.GetByValue(model.LeadStatus)
			, model.Title
			, model.Company
			, model.Mobile
			, model.Phone
			, Rating.GetByValue(model.Rating)
			, model.Country
			, model.State
			, model.City
			, model.Street
			, Industry.GetByValue(model.Industry)
			, model.AnnualRevenue
			, LeadSource.GetByValue(model.LeadSource)
			, model.PostalCode
			, model.NumberOfEmployees
			, model.Website
			, model.Description
			);

			await UnitOfWork.SaveAsync();

			result.WithData( leadModel );

			return Ok(result);
		}
		catch (Exception ex)
		{
			result.AddErrorMessage(ex.Message);
			return BadRequest(result);
		}

	}

}