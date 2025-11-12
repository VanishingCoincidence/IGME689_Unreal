// Fill out your copyright notice in the Description page of Project Settings.


#include "APIMapCreator.h"
#include "ArcGISMapsSDK/Actors/ArcGISMapActor.h"
#include "ArcGISMapsSDK/Components/ArcGISLocationComponent.h"
#include "ArcGISMapsSDK/Components/ArcGISMapComponent.h"
#include "Kismet/GameplayStatics.h"

// Sets default values
AAPIMapCreator::AAPIMapCreator()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	
	locationComponent = CreateDefaultSubobject<UArcGISLocationComponent>("LocationComponent");
	locationComponent->SetupAttachment(GetRootComponent());

	

}

// Called when the game starts or when spawned
void AAPIMapCreator::BeginPlay()
{
	Super::BeginPlay();

	const auto map = UGameplayStatics::GetActorOfClass(GetWorld(), AArcGISMapActor::StaticClass());
	mapComponent = Cast<AArcGISMapActor>(map)->GetMapComponent();
	
}

void AAPIMapCreator::SetOrigin(UArcGISPoint* NewPosition)
{
	if (!mapComponent)
	{
		return;
	}

	mapComponent->SetOriginPosition(NewPosition);
}

// Called every frame
void AAPIMapCreator::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

