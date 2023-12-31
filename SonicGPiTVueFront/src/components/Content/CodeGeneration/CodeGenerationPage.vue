<script setup lang="ts">
import { onMounted, ref, reactive, computed } from "vue";
import type { FormInstance } from "ant-design-vue";
import type { Rule } from "ant-design-vue/es/form";
import { NotificationError, NotificationSuccess } from "@/utils/notifications";
import { useCodeGenerationsStore } from "@/stores/codegenerations";
import { storeToRefs } from "pinia";
import CodeRequest from "@/model/CodeRequest";

const codegenerationsStore = useCodeGenerationsStore();
const { generationStrategies, loading, generatingCode, errorMessages } =
	storeToRefs(codegenerationsStore);

const fetchStrategies = async () => {
	try {
		await codegenerationsStore.getGenerationStrategiesAsync();
	} catch (error) {
		console.error(error);
	} finally {
	}
};

onMounted(async () => {
	await fetchStrategies();
});

// ---------------- FORM ----------------

const currentCodeSpan = computed(() => {
	return formState.newCode ? 6 : 18;
});

const newCodeSpan = computed(() => {
	return formState.newCode ? 18 : 6;
});

interface FormState {
	currentCode: string;
	newCode: string;
	inputText: string;
	strategy: string;
	useExpensiveModel: boolean;
}

const formState = reactive<FormState>({
	currentCode: "",
	newCode: "",
	inputText: "",
	strategy: "",
	useExpensiveModel: false,
});

const formRef = ref<FormInstance>();

const rules: Record<string, Rule[]> = {
	inputText: [{ required: true, message: "Input text is required." }],
};

const onValidationFailed = (errorInfo: any) => {
	console.log("Validation failed:", errorInfo);
};

const submitForm = async () => {
	let codeRequest: CodeRequest | null = null;

	try {
		codeRequest = getCodeRequest(formState);
	} catch {
		errorMessages.value.push("Failed to create claim from form data.");
		console.log("Failed to create claim from form data.");
		return;
	}

	try {
		const newCode = await codegenerationsStore.generateCodeWithStrategy(
			codeRequest
		);

		formState.newCode = newCode;
	} catch (error) {
		errorMessages.value.push("Failed to create claim from form data.");
		console.log("Failed to create claim from form data.");
	}
};

function getCodeRequest(form: FormState): CodeRequest {
	const codeRequest = new CodeRequest();

	codeRequest.currentCode = form.currentCode;
	codeRequest.generationMethod = form.strategy;
	codeRequest.userInput = form.inputText;
	codeRequest.useExpensiveModel = form.useExpensiveModel;

	return codeRequest;
}

async function copyAndMoveUp() {
	try {
		await navigator.clipboard.writeText(formState.newCode);

		NotificationSuccess("Code Copied!", `Code copied to clipboard`);

		formState.currentCode = formState.newCode;
		formState.newCode = "";
	} catch (err) {
		console.error("Failed to copy text: ", err);
		NotificationError(
			"Failed copy code",
			`Code couldn't be copied to clipboard.`
		);
	}
}
</script>

<template>
	<div class="header">
		<h1>Sonic GPiT</h1>
	</div>

	<div>
		<a-form
			:model="formState"
			:rules="rules"
			ref="formRef"
			layout="vertical"
			@finish="submitForm"
			@finishFailed="onValidationFailed"
		>
			<a-row>
				<a-col :span="6"></a-col>
				<a-col :span="12">
					<a-form-item
						label="Input Text"
						name="inputText"
					>
						<a-input-group compact>
							<a-input
								v-model:value="formState.inputText"
								placeholder="Please enter text input to GPT"
								required
							/>
						</a-input-group>
					</a-form-item>
				</a-col>
				<a-col :span="6"></a-col>
			</a-row>
			<a-row>
					<a-col :span="currentCodeSpan" key="currentCodeSpan">
						<a-form-item
							label="Current Code"
							name="currentCode"
						>
							<a-input-group compact>
								<a-textarea
									class="code-textarea"
									v-model:value="formState.currentCode"
									placeholder="Please enter the current Sonic Pi code"
								/>
							</a-input-group>
						</a-form-item>
					</a-col>
					<a-col :span="newCodeSpan" key="newCodeSpan">
						<a-form-item
							label="New code"
							name="newCode"
						>
							<a-input-group compact>
								<a-textarea
									class="code-textarea"
									v-model:value="formState.newCode"
									placeholder="Code generated by GPT"
								/>
							</a-input-group>
						</a-form-item>
					</a-col>
			</a-row>
			<a-row>
				<a-col :span="16">
					<div class="form-buttons">
						<a-form-item class="form-button">
							<p class="form-button-label">Use expensive model</p>
						</a-form-item>
						<a-form-item class="form-button">
							<a-switch v-model:checked="formState.useExpensiveModel" />
						</a-form-item>
						<a-form-item class="form-button">
							<a-button
								type="primary"
								html-type="submit"
								:loading="generatingCode"
								>Submit</a-button
							>
						</a-form-item>
					</div>
				</a-col>
			</a-row>
			<a-row>
				<a-col :span="6"></a-col>
				<a-col :span="12">
					<div v-for="msg in errorMessages">
						<a-typography-text type="danger">{{msg}}</a-typography-text>
					</div>
				</a-col>
				<a-col :span="6"></a-col>
			</a-row>
			<a-row v-if="formState.newCode">
				<a-col :span="16">
					<div class="form-buttons">
						<a-form-item>
							<a-button
								class="form-button"
								@click="copyAndMoveUp"
								>Copy and move to current</a-button
							>
						</a-form-item>
					</div>
				</a-col>
			</a-row>
		</a-form>
	</div>

	<p>Loading: {{ loading }}</p>
	<p>GeneratingCode: {{ generatingCode }}</p>
</template>

<style scoped>
.form-buttons {
	display: flex;
	align-self: center;
	justify-content: end;
}

.form-button {
	align-self: center;
	margin-right: 1vw;
}

.form-button-label {
	margin-top: 2vh;
}

.code-textarea {
	min-height: 10.5em; /* 1.5em * 7 */
}
</style>
