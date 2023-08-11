class CodeRequest {
	public currentCode: string;
	public generationMethod: string;
	public userInput: string;

	constructor() {
		this.currentCode = "";
		this.generationMethod = "";
		this.userInput = "";
	}
}

export default CodeRequest;
