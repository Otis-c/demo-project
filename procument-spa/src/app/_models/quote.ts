export interface Quote {
  id: number;
  requisitionId: number;
  description: string;
  company: string;
  submittedBy: string;
  amount: number;
  filePath: string;
  status: string;
}
