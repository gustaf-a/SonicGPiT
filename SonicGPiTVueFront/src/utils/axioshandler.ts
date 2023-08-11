import axios from "axios";
import type { AxiosRequestConfig, AxiosResponse } from "axios";
import BackendResponse from "@/model/BackendResponse";

export default async function requestHandler<T>(
	requestConfig: AxiosRequestConfig,
	errorMessageBase: string
): Promise<BackendResponse<T>> {
	const resultResponse = new BackendResponse<T>();

	try {
		const response: AxiosResponse<BackendResponse<T>> = await axios(
			requestConfig
		);

		if (!response || !response.data) {
			resultResponse.messages = [errorMessageBase + ": No response received."];
			resultResponse.isSuccess = false;
			return resultResponse;
		}

		const receivedResponse = response.data;

		resultResponse.messages = receivedResponse.messages || [];
		resultResponse.isSuccess = receivedResponse.isSuccess;
		
		if (receivedResponse.data) {
			resultResponse.data = receivedResponse.data as T;
			if(!resultResponse.data){
				resultResponse.messages.push(errorMessageBase + ": Failed to parse data from response.");
			}
		} else {
			resultResponse.messages.push(errorMessageBase + ": No data received.");
		}
		
		return resultResponse;
	} catch (error) {
		resultResponse.isSuccess = false;
		resultResponse.messages = [];

		if (axios.isAxiosError(error)) {
			if (error.response) {
				const axiosResponse = error.response;

				const backendResponse = axiosResponse.data as BackendResponse<T>; 
				if(backendResponse){
					if(backendResponse.messages)
						resultResponse.messages = backendResponse.messages;
				
					if(backendResponse.data)
						resultResponse.data = backendResponse.data;
				} else {
					resultResponse.messages.push(
						`Server responded with status code: ${axiosResponse.status}`
					);
				}
			} else if (error.request) {
				resultResponse.messages.push(
					`No response received from server: ${error.message}`
				);
			} else {
				resultResponse.messages.push(
					`Error in setting up the request: ${error.message}`
				);
			}
		} else {
			resultResponse.messages.push(errorMessageBase);
			resultResponse.messages.push(
				`Unexpected error: ${
					error instanceof Error ? error.message : String(error)
				}`
			);
		}
	}

	return resultResponse;
}