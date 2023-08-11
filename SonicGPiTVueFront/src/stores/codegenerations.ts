import { ref } from 'vue'
import { defineStore } from 'pinia'

import { ErrorMessages } from "@/utils/errors";

import axios from "axios";
import requestHandler from "@/utils/axioshandler";

import CodeRequest from "@/model/CodeRequest"
import CodeResponse from "@/model/CodeResponse"

const { VITE_API_BASE_URL } = import.meta.env;

export const useCodeGenerationsStore = defineStore('codegenerations', () => {
  
    const errorMessages = ref<string[]>([]);
    const loading = ref(false);
    const generatingCode = ref(false);

    const generationStrategies = ref<string[]>([]);

    async function getGenerationStrategiesAsync(): Promise<void> {
      if(generationStrategies.value.length > 0) return;
      
      try {
        loading.value = true;
  
        const response = await axios.get(`${VITE_API_BASE_URL}/api/code/strategies`);
  
        if (response.status !== 200) {
          errorMessages.value.push(ErrorMessages.DATA_FETCH_ERROR);
          return;
        }
  
        generationStrategies.value = response.data;
      } catch (error) {
        errorMessages.value.push(`${ErrorMessages.DATA_FETCH_ERROR}: ${
          error instanceof Error ? error.message : String(error)
        }`);
      } finally {
        loading.value = false;
      }
    }

    async function generateCodeWithStrategy(codeRequest: CodeRequest): Promise<string> {
        errorMessages.value = [];

        generatingCode.value = true;
  
        let codeResponse: CodeResponse | null = null;

        try {
          loading.value = true;
  
          const backendResponse = await requestHandler<CodeResponse>(
            {
              method: "post",
              url: `${VITE_API_BASE_URL}/api/code`,
              data: codeRequest
            },
            ErrorMessages.CODE_GENERATION_ERROR
          );
  
          if (backendResponse.messages) {
            errorMessages.value = errorMessages.value.concat(
              backendResponse.messages
            );
          }
  
          if (backendResponse.data) {
              codeResponse = backendResponse.data || null;
          }
        } catch (error) {
          // Any unexpected error
          errorMessages.value.push(
            `Unexpected error: ${
              error instanceof Error ? error.message : String(error)
            }`
          );
        } finally {
          loading.value = false;
        }
  
        return codeResponse?.GeneratedCode || "";
    }

    return {
      getGenerationStrategiesAsync,
      generateCodeWithStrategy
    };
})
