using Domain.Abstract.Repository;
using Domain.Abstract.Service;
using Domain.Models;
using Domain.Models.Message;
using iFinancing360.Service.Helper;
using Microsoft.Extensions.Logging;
using Service.Messaging.Abstraction.IConsumerService;
using System.Text.Json;


namespace Service.Messaging.ConsumerMessage;
public class AgreementIncentiveMarketingConsumerMessage : BaseMessagingService, IAgreementIncentiveMarketingConsumerMessage
{
  private readonly ILogger<AgreementIncentiveMarketingConsumerMessage> _logger;
  private readonly IInterfaceAgreementIncentiveMarketingRepository _repoInterfaceAgreementIncentiveMarketing;
  private readonly IInterfaceAgreementFeeRepository _repoInterfaceAgreementFee;
  private readonly IInterfaceAgreementRefundRepository _repoInterfaceAgreementRefund;

  public AgreementIncentiveMarketingConsumerMessage(ILogger<AgreementIncentiveMarketingConsumerMessage> logger, IInterfaceAgreementIncentiveMarketingRepository repoInterfaceAgreementIncentiveMarketing, IInterfaceAgreementFeeRepository repoInterfaceAgreementFee, IInterfaceAgreementRefundRepository repoInterfaceAgreementRefund)
  {
    _logger = logger;
    _repoInterfaceAgreementIncentiveMarketing = repoInterfaceAgreementIncentiveMarketing;
    _repoInterfaceAgreementFee = repoInterfaceAgreementFee;
    _repoInterfaceAgreementRefund = repoInterfaceAgreementRefund;
  }

  public async Task ConsumeRequest(MessageAgreementIncentiveMarketing message)
  {
    try
    {

      await ConsumeAgreementIncentiveMarketing(message);
    }
    catch (Exception e)
    {
      _logger.LogWarning("Kafka consumer Error: {Message}", e.Message);
      throw;
    }

  }

  private async Task ConsumeAgreementIncentiveMarketing(MessageAgreementIncentiveMarketing message)
  {
    using var connection = await GetDbConnection();
    using var transaction = connection.BeginTransaction();
    try
    {
      int result = 0;

      InterfaceAgreementIncentiveMarketing agreementIncentiveMarketing = new()
      {
        ID = GUID(),
        CreDate = message.CreDate,
        CreBy = message.CreBy,
        CreIPAddress = message.CreIPAddress,
        ModDate = message.ModDate,
        ModBy = message.ModBy,
        ModIPAddress = message.ModIPAddress,
        ApplicationMainID = message.ApplicationMainID,
        AgreementNo = message.AgreementNo,
        ClientID = message.ClientID,
        ClientNo = message.ClientNo,
        ClientName = message.ClientName,
        ApprovedDate = message.ApprovedDate,
        DisbursementDate = message.DisbursementDate,
        IncentivePeriod = message.IncentivePeriod,
        PaymentMethod = message.PaymentMethod,
        CurrencyID = message.CurrencyID,
        CurrencyCode = message.CurrencyCode,
        CurrencyDesc = message.CurrencyDesc,
        NetFinance = message.NetFinance,
        InterestRate = message.InterestRate,
        CostRate = message.CostRate,
        InterestMargin = message.InterestMargin,
        InsuranceRate =  message.InsuranceRate,
        InterestAmount = message.InterestAmount,
        CostAmount = message.CostAmount,
        InterestMarginAmount = message.InterestMarginAmount,
        TotalInsurancePremiAmount = message.TotalInsurancePremiAmount,
        CCYRate = message.CCYRate,
        CommissionRate = message.CommissionRate,
        VendorID = message.VendorID,
        VendorCode = message.VendorCode,
        VendorName = message.VendorName,
        AgentID = message.AgentID,
        AgentCode = message.AgentCode,
        AgentName = message.AgentName,
        JobStatus = "HOLD",
      };
      result += await _repoInterfaceAgreementIncentiveMarketing.Insert(transaction, agreementIncentiveMarketing);
      
      if (message.AgreementFees != null && message.AgreementFees.Any())
      {
        foreach (var item in message.AgreementFees)
        {
          InterfaceAgreementFee agreementFee = new()
          {
            ID = GUID(),
            CreDate = message.CreDate,
            CreBy = message.CreBy,
            CreIPAddress = message.CreIPAddress,
            ModDate = message.ModDate,
            ModBy = message.ModBy,
            ModIPAddress = message.ModIPAddress,
            AgreementIncentiveID = agreementIncentiveMarketing.ID,
            FeeID = item.FeeID,
            FeeCode = item.FeeCode,
            FeeName = item.FeeName,
            FeeAmount = item.FeeAmount,
            FeeRate = item.FeeRate,
            FeePaymentType = item.FeePaymentType,
            FeePaidAmount = item.FeePaidAmount,
            FeeReduceDisburseAmount = item.FeeReduceDisburseAmount,
            FeeCapitalizeAmount = item.FeeCapitalizeAmount,
            InsuranceYear = item.InsuranceYear,
            Remarks = item.Remarks,
            IsInternalIncome = item.IsInternalIncome,
            JobStatus = "HOLD",
          };
          result += await _repoInterfaceAgreementFee.Insert(transaction, agreementFee);
        }
      }

      if (message.AgreementRefunds != null && message.AgreementRefunds.Any())
      {
        foreach (var item in message.AgreementRefunds)
        {
          InterfaceAgreementRefund agreementRefund = new()
          {
            ID = GUID(),
            CreDate = message.CreDate,
            CreBy = message.CreBy,
            CreIPAddress = message.CreIPAddress,
            ModDate = message.ModDate,
            ModBy = message.ModBy,
            ModIPAddress = message.ModIPAddress,
            AgreementIncentiveID = agreementIncentiveMarketing.ID,
            RefundID = item.RefundID,
            RefundCode = item.RefundCode,
            RefundDesc = item.RefundName,
            RefundAmount = item.RefundAmount,
            RefundRate = item.RefundRate,
            CalculateBy = item.CalculateBy,
            JobStatus = "HOLD",
          };
          result += await _repoInterfaceAgreementRefund.Insert(transaction, agreementRefund);
        }
      }

      transaction.Commit();
    }
    catch (Exception)
    {
      transaction.Rollback();
      throw;
    }
  }
}