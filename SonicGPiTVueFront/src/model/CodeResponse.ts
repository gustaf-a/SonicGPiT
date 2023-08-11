class CodeResponse {
	public isSuccess: boolean;
	public GeneratedCode: string;
	public errorMessages: string[];

	constructor() {
		this.isSuccess = false;
		this.GeneratedCode = "";
		this.errorMessages = [];
	}
}

export default CodeResponse;
