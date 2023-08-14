import { notification } from "ant-design-vue";

type NotificationType = "success" | "error" | "info" | "warning";

export function NotificationSuccess(title: string, message: string) {
	openNotificationWithIcon(title, message, "success");
}

export function NotificationError(title: string, message: string) {
	openNotificationWithIcon(title, message, "error");
}

export function NotificationWarning(title: string, message: string) {
	openNotificationWithIcon(title, message, "info");
}

export function NotificationInfo(title: string, message: string) {
	openNotificationWithIcon(title, message, "warning");
}

const openNotificationWithIcon = (
	title: string,
	message: string,
	notificationType: NotificationType
) => {
	if (notificationType in notification) {
		notification[notificationType]({
			message: title,
			description: message,
		});
	} else {
		console.log("Failed to create notification with type: " + notificationType);
	}
};
