class CodeResponse {
	public isSuccess: boolean;
	public generatedCode: string;
	public errorMessages: string[];

	constructor() {
		this.isSuccess = false;
		this.generatedCode = "";
		this.errorMessages = [];
	}
}

export default CodeResponse;
