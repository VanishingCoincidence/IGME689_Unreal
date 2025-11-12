// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Http.h"
#include "FeatureLayerQuery.generated.h"

UCLASS()
class IGME689_API AFeatureLayerQuery : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AFeatureLayerQuery();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
	virtual void OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bSucceeded);
	virtual void ProcessRequest();

private:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (AllowPrivateAccess = "true"))
	FString webLink = "";
};

