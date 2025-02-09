export class Transaction_Dto {
    public id: number = 0;
    public amount: number = 0;
    public currency: string = "";
    public payerName: string = "";
    public cardNumber: string = "";
    public expiryDate: string = "";
    public cvv: string = "";
    public accountNumber: string = "";
    public bankName: string = "";
    public paymentMethod: string = "";
    public paymentStatus: string = "";
    public transactionDateTime: Date = new Date();
    public referenceNo: number = 0;
    public userId: number = 0;
    public user: any = null;


}